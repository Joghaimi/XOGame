using CatchyGame.Data;
using Library.Model;
using Microsoft.EntityFrameworkCore;

namespace CatchyGame.Repository
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly ApplicationDbContext _dbcontext;
        public ScoreRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public Score GetTopScoreThisDay()
        {
            var today = DateTime.Now.Date;
            Score todayScore = _dbcontext.Score.Where(e => e.TimeStamp.Date == today).OrderByDescending(e => e.TeamScore).FirstOrDefault();
            return todayScore;
        }
        public Score GetTopScoreThisMonth()
        {
            var lastMonth = DateTime.Today.AddDays(-30); // Calculate the date 30 days ago
            Score lastMonthTopScore = _dbcontext.Score
                .Where(e => e.TimeStamp.Date >= lastMonth && e.TimeStamp.Date <= DateTime.Today)
                .OrderByDescending(e => e.TeamScore)
                .FirstOrDefault();
            return lastMonthTopScore;
        }
        public Score GetTopScoreThisWeek()
        {
            var lastWeek = DateTime.Today.AddDays(-7);
            Score lastWeekTopScore = _dbcontext.Score.Where(e => e.TimeStamp.Date >= lastWeek && e.TimeStamp.Date <= DateTime.Today).OrderByDescending(e => e.TeamScore).FirstOrDefault();
            return lastWeekTopScore;
        }
        public bool SaveScore(Score score)
        {
            _dbcontext.Score.Add(score);
            return _dbcontext.SaveChanges() > 0;
        }
        public async Task<bool> removeAllAsync()
        {
            var allScores = await _dbcontext.Score.ToListAsync();
            _dbcontext.Score.RemoveRange(allScores);
            await _dbcontext.SaveChangesAsync();
            return _dbcontext.SaveChanges() > 0;
        }

        public List<Score> TopFiveScore()
        {
            List<Score> topFiveScore = _dbcontext.Score
               .OrderByDescending(e => e.TeamScore)
               .Take(5).ToList();
            return topFiveScore;
        }

        public List<Score> TopThreeScoreToday()
        {
            List<Score> topFiveScore =
                _dbcontext.Score.Where(e => e.TimeStamp.Date == DateTime.Now.Date)
              .OrderByDescending(e => e.TeamScore)
              .Take(5).ToList();
            return topFiveScore;
        }

        public List<Score> TopThreeScoreMonth()
        {
            var lastMonth = DateTime.Today.AddDays(-30);

            List<Score> topFiveScore =
            _dbcontext.Score.Where(e => e.TimeStamp.Date >= lastMonth && e.TimeStamp.Date <= DateTime.Today)
            .OrderByDescending(e => e.TeamScore)
            .Take(3).ToList();
            return topFiveScore;
        }

        public List<Score> TopThreeScoreWeek()
        {

            var lastWeek = DateTime.Today.AddDays(-7);

            List<Score> topFiveScore =
            _dbcontext.Score.Where(e => e.TimeStamp.Date >= lastWeek && e.TimeStamp.Date <= DateTime.Today)
            .OrderByDescending(e => e.TeamScore)
            .Take(3).ToList();
            return topFiveScore;
        }
    }
}
