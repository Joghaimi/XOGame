using Library.Model;

namespace CatchyGame.Repository
{
    public interface IScoreRepository
    {
        public Score GetTopScoreThisMonth();
        public Score GetTopScoreThisWeek();
        public Score GetTopScoreThisDay();
        public List<Score> TopFiveScore();
        public List<Score> TopThreeScoreToday();
        public List<Score> TopThreeScoreMonth();
        public List<Score> TopThreeScoreWeek();
        public bool SaveScore(Score score);
        public Task<bool> removeAllAsync();

    }
}
