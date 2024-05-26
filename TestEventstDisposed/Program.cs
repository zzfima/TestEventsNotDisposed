using System;

namespace TestEventsDisposed
{
    public class Publisher
    {
        public event EventHandler SomeEventHappens;

        public void SomeFunc()
        {
            SomeEventHappens?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Subscriber : IDisposable
    {
        private Publisher _publisher;
        private int _id;
        private bool _disposed = false;

        public Subscriber(Publisher publisher, int id)
        {
            _publisher = publisher;
            _id = id;
            _publisher.SomeEventHappens += SomeEventHappens;
        }

        private void SomeEventHappens(object sender, EventArgs e)
        {
            Console.WriteLine($"Subscriber {_id} handling Elapsed event.");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Subscriber()
        {
            Console.WriteLine($"Subscriber {_id} finalized.");
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Console.WriteLine($"Subscriber {_id} disposed.");
                if (disposing)
                    _publisher.SomeEventHappens -= SomeEventHappens;

                _disposed = true;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var publisher = new Publisher();

            for (int i = 1; i <= 113; i++)
            {
                using (var subscriber = new Subscriber(publisher, i))
                {
                    Console.WriteLine($"Subscriber {i} created. Press Enter to create next subscriber...");
                    Console.ReadLine();
                    publisher.SomeFunc();
                }
            }

            Console.WriteLine("Press Enter to exit and see remaining notifications...");
            Console.ReadLine();
        }
    }
}
