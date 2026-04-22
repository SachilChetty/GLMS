namespace GLMS.Patterns
{
    public interface IObserver
    {
        void Update(string message);
    }

    public class NotificationService : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"Notification: {message}");
        }
    }
}
