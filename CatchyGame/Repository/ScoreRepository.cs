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

            // Remove all entities
            _dbcontext.Score.RemoveRange(allScores);

            // Save changes to the database
            await _dbcontext.SaveChangesAsync();
            return _dbcontext.SaveChanges() > 0;
        }
    }
}
