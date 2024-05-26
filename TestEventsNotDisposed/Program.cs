using System;

namespace TestEventsNotDisposed
{
    public class Publisher
    {
        public event EventHandler SomeEventHappens;

        public void SomeFunc()
        {
            SomeEventHappens?.Invoke(this, EventArgs.Empty);
        }
    }

    public class Subscriber
    {
        private Publisher _publisher;
        private int _id;

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

        ~Subscriber()
        {
            Console.WriteLine($"Subscriber {_id} finalized.");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var publisher = new Publisher();

            for (int i = 1; i <= 113; i++)
            {
                var subscriber = new Subscriber(publisher, i);
                Console.WriteLine($"Subscriber {i} created. Press Enter to create next subscriber...");
                Console.ReadLine();
                publisher.SomeFunc();
            }

            Console.WriteLine("Press Enter to exit and see remaining notifications...");
            Console.ReadLine();
        }
    }
}
