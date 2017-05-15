namespace LeapConsole
{
    public struct TotalVelocity
    {
        public float HorizontalVelocity { get; }
        public float VerticalVelocity { get; }

        public TotalVelocity(float horizontal, float vertical)
        {
            HorizontalVelocity = horizontal;
            VerticalVelocity = vertical;
        }

        public override string ToString()
        {
            return $"{{ {HorizontalVelocity} ; {VerticalVelocity} }}";
        }
    }
}
