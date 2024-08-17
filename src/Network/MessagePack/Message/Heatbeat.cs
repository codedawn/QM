using MessagePack;

namespace QM
{
    [MessageDispatch("connector")]
    [MessageIndex(0)]
    [MessagePackObject]
    public class Heatbeat : INotify
    {
    }
}
