namespace LeapConsole
{
    public struct VelocityInfo
    {
        public float HorizontalVelocity { get; }
        public float VerticalVelocity { get; }
        public float ZVelocity { get; }

        public VelocityInfo(float totalHorizontal, float totalVertical, float totalZ)
        {
            HorizontalVelocity = totalHorizontal;
            VerticalVelocity = totalVertical;
            ZVelocity = totalZ;
        }

        public override string ToString()
        {
            return $"{{ {HorizontalVelocity} ; {VerticalVelocity} }}";
        }
    }
}
