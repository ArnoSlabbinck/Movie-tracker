using Eindproject.Data;
using Eindproject.Domain;
using Eindproject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindproject.Controllers
{

    public class CommentController : Controller
    {
        private ICommentRepository commentRepository;

        public CommentController(ICommentRepository commentRepository)
        {
            this.commentRepository = commentRepository;
         
        }


        public IActionResult DeleteComment(Comment comment)
        {
            commentRepository.DeleteComment(comment.CommentId);
            return RedirectToAction("Movie/ViewFilmSerie");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CommentViewModel comment)
        {
            var editComment = commentRepository.GetComment(comment.Comment_Id);
            editComment.Comment_Message = comment.Comment_Message;
            commentRepository.UpdateComment(editComment);
            return RedirectToAction("Movie/ViewFilmSerie");
        }
        [HttpGet]
        public IActionResult Edit(MovieCommentViewModel movieCommentViewModel)
        {
            CommentViewModel commentView = new CommentViewModel();
            return PartialView("CommentModalPartial", commentView);

        }

        /// <summary>
        /// Edit comment from User with a modal
        /// </summary>
        /// <param name="CommentId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult EditComment(int CommentId)
        {

            var comment = commentRepository.GetComment(CommentId);
            CommentViewModel commentView = new CommentViewModel();
            commentView.Comment_Id = CommentId;
            commentView.Comment_Message = comment.Comment_Message;
            commentView.Created_Date = comment.Created_Date;
            TempData["id"] = CommentId;
            return PartialView("CommentModalPartial", commentView);
        }

        /// <summary>
        /// Post new Comment to database
        /// </summary>
        /// <param name="Comment_Id"></param>
        /// <param name="Comment_Message"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditComment(int Comment_Id, string Comment_Message)
        {
            if (string.IsNullOrEmpty(Comment_Message))
            {
                return PartialView("CommentModalPartial", new CommentViewModel());
            }

            Comment comment = commentRepository.GetComment(Comment_Id);
            comment.Comment_Message = Comment_Message;
            comment.Created_Date = DateTime.Now;
            commentRepository.UpdateComment(comment);
            return PartialView("CommentModalPartial", new CommentViewModel());
        }

    }
}
