namespace SignalR.DtoLayer.Dtos.RapidApiDtos
{
    public class RootTasty
    {
        public List<ResultTastyDto> Results { get; set; }
    }
    public class ResultTastyDto
    {
        public Nutrition nutrition { get; set; }
        public string name { get; set; }
        public int total_time_minutes { get; set; }
        public string original_video_url { get; set; }
        public string thumbnail_url { get; set; }

        public class Nutrition
        {
            public int carbohydrates { get; set; }
            public int fiber { get; set; }
            public int protein { get; set; }
            public int fat { get; set; }
            public int calories { get; set; }
            public int sugar { get; set; }
        }
    }
}