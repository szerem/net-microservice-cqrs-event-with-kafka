using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
    public class ICommentRepository
    {
        Task CreateAsync(CommentEntity comment);
        Task UpdateAsync(CommentEntity comment);
        Task<CommentEntity> GetByIdAsync(Guid commentId);
        Task DeleteAsync(Guid commentId);
        
    }
}