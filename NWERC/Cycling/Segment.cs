namespace NWERC.Cycling
{
    public class Segment
    {
        public float LengthInMeters { get; set; }
        public float RedLightTime { get; set; }
        public float GreenLightTime { get; set; }

        public Segment(float lengthInMeters, float redLightTime, float greenLightTime)
        {
            LengthInMeters = lengthInMeters;
            RedLightTime = redLightTime;
            GreenLightTime = greenLightTime;
        }
    }
}