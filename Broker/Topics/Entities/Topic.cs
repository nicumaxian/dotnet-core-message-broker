namespace Broker.Topics.Entities
{
    public class Topic
    {
        public string Identifier { get; }

        public Topic(string identifier)
        {
            Identifier = identifier;
        }

        public override string ToString()
        {
            return $"Topic({Identifier})";
        }

        public override bool Equals(object obj)
        {
            if (obj is Topic topic)
            {
                return Identifier.Equals(topic.Identifier);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Identifier.GetHashCode();
        }
    }
}