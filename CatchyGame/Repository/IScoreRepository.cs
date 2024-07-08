using Library.Model;

namespace CatchyGame.Repository
{
    public interface IScoreRepository
    {
        public Score GetTopScoreThisMonth();
        public Score GetTopScoreThisWeek();
        public Score GetTopScoreThisDay();
        public bool SaveScore(Score score);
        public Task<bool> removeAllAsync();

    }
}
