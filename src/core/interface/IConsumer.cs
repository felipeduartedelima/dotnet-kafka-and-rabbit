namespace core.interfaces;
public interface IConsumer
{
    public void Consume(string topic, ICallbackDelegate callback);
}
