﻿using System;
using Logs.Data.Contracts;
using Logs.Factories;
using Logs.Models;
using Logs.Providers.Contracts;
using Logs.Services.Contracts;

namespace Logs.Services
{
    public class CommentService : ICommentService
    {
        private readonly ILogService logService;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ICommentFactory commentFactory;
        private readonly IUserService userService;
        private readonly IRepository<Comment> commentRepository;
        private readonly IUnitOfWork unitOfWork;

        public CommentService(ILogService logService,
            IDateTimeProvider dateTimeProvider,
            IUserService userService,
            ICommentFactory commentFactory,
            IRepository<Comment> commentRepository,
            IUnitOfWork unitOfWork)
        {
            if (logService == null)
            {
                throw new ArgumentNullException(nameof(logService));
            }

            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }

            if (dateTimeProvider == null)
            {
                throw new ArgumentNullException(nameof(dateTimeProvider));
            }

            if (commentFactory == null)
            {
                throw new ArgumentNullException(nameof(commentFactory));
            }

            if (commentRepository == null)
            {
                throw new ArgumentNullException(nameof(commentRepository));
            }

            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            this.logService = logService;
            this.dateTimeProvider = dateTimeProvider;
            this.userService = userService;
            this.commentFactory = commentFactory;

            this.commentRepository = commentRepository;
            this.unitOfWork = unitOfWork;
        }

        public Comment EditComment(int commentId, string newContent)
        {
            var comment = this.commentRepository.GetById(commentId);

            if (comment != null)
            {
                comment.Content = newContent;

                this.commentRepository.Update(comment);
                this.unitOfWork.Commit();
            }

            return comment;
        }

        public void AddCommentToLog(string content, int logId, string userId)
        {
            var currentDate = this.dateTimeProvider.GetCurrentTime();

            var user = this.userService.GetUserById(userId);

            var comment = this.commentFactory.CreateComment(content, currentDate, user);

            this.logService.AddCommentToLog(logId, comment);
        }

        public bool DeleteComment(int commentId)
        {
            var comment = this.commentRepository.GetById(commentId);

            if (comment != null)
            {
                this.commentRepository.Delete(comment);
                this.unitOfWork.Commit();

                return true;
            }

            return false;
        }
    }
}
