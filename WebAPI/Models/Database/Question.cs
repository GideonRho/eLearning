using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WebAPI.Models.API.Requests.Questions;
using WebAPI.Models.Database.Enums;
using WebAPI.Models.Excel;

namespace WebAPI.Models.Database
{
    [Table("question")]
    public class Question
    {

        [Column("id")]
        public int Id { set; get; }
        
        [Column("title")]
        public string Title { set; get; }
        [Column("correct_answer")]
        public int CorrectAnswer { set; get; }
        [Column("category")]
        public ECategory Category { set; get; }
        [Column("comment_text")]
        public string CommentText { set; get; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("image")]
        public string Image { set; get; }
        [Column("comment_image")]
        public string CommentImage { set; get; }
        [Column("options")]
        public List<string> Options{ set; get; }
        [Column("infos")]
        public List<string> Infos{ set; get; }
        [Column("tags")]
        public List<string> Tags{ set; get; }
        [Column("key")]
        public string Key{ set; get; }
        
        [Column("text_id")]
        public int? TextId { set; get; }
        [ForeignKey("TextId")]
        public Text Text { set; get; }
        

        public Question()
        {
        }

        public Question(QuestionRow row, ECategory category)
        {
            Title = row.title;
            CorrectAnswer = row.CorrectAnswerIndex();
            Category = category;
            CommentText = row.comment;
            Infos = row.Infos.ToList();
            Tags = row.Tags.ToList();
            Options = row.Answers.ToList();
            Key = row.key;
            CommentImage = row.commentImage;
            Image = row.image;
        }

        public Question(CreateQuestion data)
        {
            Title = data.Title;
            Category = data.Category;
            CorrectAnswer = data.CorrectAnswer;
            CommentText = data.CommentText;
            CommentImage = data.CommentImage;
            Image = data.Image;
            Tags = data.Tags;
            Infos = data.Infos;
            Options = data.Options;
            Key = data.Key;
        }
        
    }
}