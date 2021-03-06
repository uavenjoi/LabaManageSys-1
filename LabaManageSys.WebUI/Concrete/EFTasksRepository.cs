﻿using System;
using System.Collections.Generic;
using System.Linq;
using LabaManageSys.Domain.Abstract;
using LabaManageSys.Domain.EntitiesModel;
using LabaManageSys.WebUI.Abstract;
using LabaManageSys.WebUI.Models;

namespace LabaManageSys.WebUI.Concrete
{
    public class EFTasksRepository : ITasksRepository
    {
        private IEFDbContext context;

        public EFTasksRepository(IEFDbContext cont)
        {
            this.context = cont;
        }

        public IEnumerable<TaskModel> Tasks
        {
            get
            {
                return this.context.Tasks.Select(_ => new TaskModel
                {
                    TaskId = _.TaskId,
                    Author = _.Author,
                    Level = _.Level,
                    Text = _.Text,
                    Topic = _.Topic,
                    Name = _.Name
                }).ToList();
            }
        }

        public FilterListsModel GetFilterLists()
        {
            var filterLists = new FilterListsModel
            {
                Authors = this.context.Tasks.Select(_ => _.Author).Distinct().ToList(),
                Topics = this.context.Tasks.Select(_ => _.Topic).Distinct().ToList(),
                Levels = this.context.Tasks.Select(_ => _.Level.ToString()).Distinct().ToList()
            };
            return filterLists;
        }

        public TaskModel GetTaskByName(string name)
        {
            return new TaskModel(this.context.Tasks.FirstOrDefault(_ => _.Name == name));
        }

        public IEnumerable<TaskModel> GetTasksByFilter(FilterModel filter, int page, int pageSize)
        {
            return this.context.Tasks
                .Where(_ => (filter.Author == null || filter.Author == string.Empty || _.Author == filter.Author)
                         && (filter.Topic == null || filter.Topic == string.Empty || _.Topic == filter.Topic)
                         && (filter.Level == 0 || _.Level == filter.Level))
                .OrderBy(_ => _.TaskId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(_ => new TaskModel
                {
                    TaskId = _.TaskId,
                    Author = _.Author,
                    Level = _.Level,
                    Text = _.Text,
                    Name = _.Name,
                    Topic = _.Topic
                }).ToList();
        }

        public int GetTasksCount(FilterModel filter)
        {
            return this.context.Tasks
                .Where(_ => (filter.Author == null || filter.Author == string.Empty || _.Author == filter.Author)
                         && (filter.Topic == null || filter.Topic == string.Empty || _.Topic == filter.Topic)
                         && (filter.Level == 0 || _.Level == filter.Level)).Count();
        }

        public ResultModel TaskAddAll(IEnumerable<TaskModel> tasks)
        {
            var result = new ResultModel { NonCommited = new List<string>() };
            result.IsSucceed = true;
            foreach (var task in tasks)
            {
                if (this.context.Tasks.Any(_ => _.Name == task.Name))
                {
                    result.IsSucceed = false;
                    result.NonCommited.Add(task.Name);
                }
                else
                {
                    this.TaskUpdate(task);
                    result.SucceededItems++;
                }
            }

            return result;
        }

        public TaskModel TaskDelete(int taskId)
        {
            Task taskDbentry = this.context.Tasks.Find(taskId);
            if (taskDbentry != null)
            {
                this.context.Tasks.Remove(taskDbentry);
                this.context.SaveChanges();
            }

            return new TaskModel(taskDbentry);
        }

        public void TaskUpdate(TaskModel task)
        {
            if (task.TaskId == 0)
            {
                this.context.Tasks.Add(new Task
                {
                    Author = task.Author,
                    Level = task.Level,
                    Text = task.Text,
                    Topic = task.Topic,
                    Name = task.Name
                });
            }
            else
            {
                Task taskDbentry = this.context.Tasks.Find(task.TaskId);
                if (taskDbentry != null)
                {
                    taskDbentry.Author = task.Author;
                    taskDbentry.Level = task.Level;
                    taskDbentry.Topic = task.Topic;
                    taskDbentry.Name = task.Name;
                    taskDbentry.Text = task.Text;
                }
            }

            this.context.SaveChanges();
        }

        public TaskModel GetTaskById(int taskId)
        {
            return new TaskModel(this.context.Tasks.FirstOrDefault(_ => _.TaskId == taskId));
        }

        public double GetAvgRatingByTaskId(int taskId)
        {
            if (this.context.Ratings.Where(_ => _.TaskId == taskId).Count() > 0)
            {
                return this.context.Ratings.Where(_ => _.TaskId == taskId).Average(_ => _.Evaluation);
            }
            else
            {
                return 0.0;
            }
        }

        public RatingModel GetRatingByTaskUser(string userName, int taskId)
        {
            var user = this.context.AppUsers.FirstOrDefault(_ => _.Name == userName);
            if (user != null)
            {
                var rating = this.context.Ratings.FirstOrDefault(_ => _.UserId == user.UserId && _.TaskId == taskId);
                if (rating != null)
                {
                    return new RatingModel(rating);
                }
                else
                {
                    return new RatingModel(new Rating { Comment = string.Empty, TaskId = taskId, UserId = user.UserId });
                }
            }

            return null;
        }

        public IEnumerable<RatingModel> GetRatingsByTask(int taskId, int page, int pageSize)
        {
            return this.context.Ratings.Where(_ => _.TaskId == taskId)
                .OrderBy(_ => _.RatingId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize).Select(_ => new RatingModel
                {
                    RatingId = _.RatingId,
                    Evaluation = _.Evaluation,
                    TaskId = _.TaskId,
                    UserId = _.UserId,
                    Comment = _.Comment
                }); 
        }

        public void RatingDelete(int ratingId)
        {
            if (ratingId != 0)
            {
                Rating ratingDbentry = this.context.Ratings.Find(ratingId);
                if (ratingDbentry != null)
                {
                    this.context.Ratings.Remove(ratingDbentry);
                    this.context.SaveChanges();
                }
            }
        }

        public void RatingUpdate(RatingModel rating)
        {
            if (rating.RatingId == 0)
            {
                this.context.Ratings.Add(new Rating
                {
                    Comment = rating.Comment,
                    UserId = rating.UserId,
                    TaskId = rating.TaskId,
                    Evaluation = rating.Evaluation
                });
            }
            else
            {
                Rating ratingDbentry = this.context.Ratings.Find(rating.RatingId);
                if (ratingDbentry != null)
                {
                    ratingDbentry.Comment = rating.Comment;
                    ratingDbentry.Evaluation = rating.Evaluation;
                }
            }

            this.context.SaveChanges();
        }
    }
}