namespace P7CreateRestApi.Dtos
{
    public class CurvePointDto
    {
        public int Id { get; set; }
        public byte? CurveId { get; set; }
        public double? Term { get; set; }
        public double? CurvePointValue { get; set; }
    }
}
