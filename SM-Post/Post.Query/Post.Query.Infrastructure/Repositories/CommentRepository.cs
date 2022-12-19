using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DatabaseContextFactory _contextFactory;
        public CommentRepository(DatabaseContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task CreateAsync(CommentEntity comment)
        {
            using var context = _contextFactory.CreateDbContext();
            _ = await context.Comments.AddAsync(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid commentId)
        {
            using var context = _contextFactory.CreateDbContext();
            var comment = await context.Comments.FindAsync(commentId);
            if (comment is null)
            {
                return;
            }
            context.Comments.Remove(comment);
            _ = await context.SaveChangesAsync();
        }

        public async Task<CommentEntity> GetByIdAsync(Guid commentId)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Comments
            // .AsNoTracking()
            .FirstAsync(x => x.CommentId == commentId)
            ;
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Comments.Update(comment);
            _ = await context.SaveChangesAsync();
        }
    }
}