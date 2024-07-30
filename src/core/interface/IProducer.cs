namespace core.interfaces;

public interface IProducer
{
    public void Produce(string topic, string message);
}
