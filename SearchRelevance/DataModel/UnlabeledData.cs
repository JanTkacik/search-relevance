namespace SearchRelevance.DataModel
{
    public class UnlabeledData
    {
        public int Id { get; set; }
        public double MatchedPercentage { get; set; }
        public double TitleMatchedPercentage { get; set; }
        public double DescriptionMatchedPercentage { get; set; }

        public double[] GetInputVector()
        {
            return new[] { MatchedPercentage, TitleMatchedPercentage, DescriptionMatchedPercentage };
        }

        public int GetRelevance(double output)
        {
            if (output < 0.25)
            {
                return 1;
            }
            if (output < 0.5)
            {
                return 2;
            }
            if (output < 0.75)
            {
                return 3;
            }
            return 4;
        }
    }
}
