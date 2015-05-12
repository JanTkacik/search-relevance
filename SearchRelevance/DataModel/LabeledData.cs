namespace SearchRelevance.DataModel
{
    public class LabeledData
    {
        public int Id { get; set; }
        public int Relevance { get; set; }
        public double RelevanceVariance { get; set; }
        public double MatchedPercentage { get; set; }
        public double TitleMatchedPercentage { get; set; }
        public double DescriptionMatchedPercentage { get; set; }

        public double[] GetInputVector()
        {
            return new[] {MatchedPercentage, TitleMatchedPercentage, DescriptionMatchedPercentage};
        }

        public double GetOutput()
        {
            return 0.125 + ((Relevance - 1) * 0.25);
        }
    }
}
