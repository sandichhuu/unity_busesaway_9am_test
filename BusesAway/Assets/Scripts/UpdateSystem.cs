using System.Collections.Generic;

public class UpdateSystem
{
    private const int THRESHOLD_SINGLE = 100; // count < 100, update directly
    private const int THRESHOLD_SLICE_2 = 500; // count < 500, each frame only process half.
    private const int THRESHOLD_SLICE_4 = 1000; // count < 1000, each frame only process part 1/4.

    private struct LoopNode<T>
    {
        public string Id;
        public T Action;
    }

    private readonly List<LoopNode<ILoop>> loopList = new();
    private readonly Dictionary<string, int> loopDict = new();

    private readonly List<LoopNode<IFixedLoop>> fixedLoopList = new();
    private readonly Dictionary<string, int> fixedLoopDict = new();

    private int updateFrameIndex = 0;
    private int fixedFrameIndex = 0;
    private int currentUpdateSlices = 1;
    private int currentFixedUpdateSlices = 1;

    private int GetSliceCount(int count)
    {
        if (count < THRESHOLD_SINGLE) return 1;
        if (count < THRESHOLD_SLICE_2) return 2;
        if (count < THRESHOLD_SLICE_4) return 4;

        return 1;
    }

    public void AddNode(System.Object obj)
    {
        if (obj is ISetup setup) setup.Invoke();

        if (obj is ILoop loop)
        {
            if (!this.loopDict.ContainsKey(loop.id))
            {
                this.loopDict[loop.id] = this.loopList.Count;
                this.loopList.Add(new LoopNode<ILoop> { Id = loop.id, Action = loop });
                this.currentUpdateSlices = GetSliceCount(this.loopList.Count);
            }

        }

        if (obj is IFixedLoop fixedLoop)
        {
            if (!this.fixedLoopDict.ContainsKey(fixedLoop.id))
            {
                this.fixedLoopDict[fixedLoop.id] = this.fixedLoopList.Count;
                this.fixedLoopList.Add(new LoopNode<IFixedLoop> { Id = fixedLoop.id, Action = fixedLoop });
                this.currentFixedUpdateSlices = GetSliceCount(this.fixedLoopList.Count);
            }
        }
    }

    public void RemoveNode(System.Object obj)
    {
        if (obj is IHaveId haveId)
        {
            RemoveFromList(this.loopList, this.loopDict, haveId.id);
            RemoveFromList(this.fixedLoopList, this.fixedLoopDict, haveId.id);
            this.currentUpdateSlices = GetSliceCount(this.loopList.Count);
            this.currentFixedUpdateSlices = GetSliceCount(this.fixedLoopList.Count);
        }
    }

    private void RemoveFromList<T>(List<LoopNode<T>> list, Dictionary<string, int> map, string id)
    {
        if (map.TryGetValue(id, out int index))
        {
            int lastIdx = list.Count - 1;
            if (index < lastIdx)
            {
                var lastNode = list[lastIdx];
                list[index] = lastNode;
                map[lastNode.Id] = index;
            }
            list.RemoveAt(lastIdx);
            map.Remove(id);
        }
    }

    public void OnUpdate(float dt)
    {
        int count = this.loopList.Count;
        if (count == 0) return;

        this.updateFrameIndex = (this.updateFrameIndex + 1) % this.currentUpdateSlices;
        float slicedDt = dt * this.currentUpdateSlices;

        for (int i = this.updateFrameIndex; i < count; i += this.currentUpdateSlices)
        {
            this.loopList[i].Action.Invoke(slicedDt);
        }
    }

    public void OnFixedUpdate(float fdt)
    {
        int count = this.fixedLoopList.Count;
        if (count == 0) return;

        this.fixedFrameIndex = (this.fixedFrameIndex + 1) % this.currentFixedUpdateSlices;
        float slicedFdt = fdt * this.currentFixedUpdateSlices;

        for (int i = this.fixedFrameIndex; i < count; i += this.currentFixedUpdateSlices)
        {
            this.fixedLoopList[i].Action.Invoke(slicedFdt);
        }
    }
}