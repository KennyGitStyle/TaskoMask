﻿using NSubstitute;
using TaskoMask.BuildingBlocks.Application.Bus;
using TaskoMask.BuildingBlocks.Test.TestBase;
using TaskoMask.Services.Tasks.Write.Domain.Data;
using TaskoMask.Services.Tasks.Write.Domain.Services;
using TaskoMask.Services.Tasks.Write.Tests.Base.TestData;

namespace TaskoMask.Services.Tasks.Write.Tests.Unit.Fixtures
{
    public abstract class TestsBaseFixture : UnitTestsBase
    {

        protected IMessageBus MessageBus;
        protected IInMemoryBus InMemoryBus;
        protected ITaskAggregateRepository TaskAggregateRepository;
        protected ITaskValidatorService TaskValidatorService;
        protected List<Domain.Entities.Task> Tasks;



        ///// <summary>
        ///// 
        ///// </summary>
        protected override void FixtureSetup()
        {
            CommonFixtureSetup();

            TestClassFixtureSetup();
        }



        /// <summary>
        /// 
        /// </summary>
        private void CommonFixtureSetup()
        {
            MessageBus = Substitute.For<IMessageBus>();

            InMemoryBus = Substitute.For<IInMemoryBus>();

            Tasks = GenerateTasksList();

            TaskValidatorService = Substitute.For<ITaskValidatorService>();
            TaskValidatorService.TaskHasUniqueName(taskId: Arg.Any<string>(), boardId: Arg.Any<string>(), taskTitle: Arg.Any<string>()).Returns(args =>
            {
                return !Tasks.Any(o => o.BoardId.Value == (string)args[1] && o.Id != (string)args[0] && o.Title.Value == (string)args[2]);
            });
            TaskValidatorService.CanAddNewTaskToBoard(boardId: Arg.Any<string>(), maxTasksCount: Arg.Any<int>()).Returns(args =>
            {
                var tasksCount = Tasks.Count(t => t.BoardId.Value == (string)args[0]);
                return tasksCount < (int)args[1];
            });

            TaskAggregateRepository = Substitute.For<ITaskAggregateRepository>();
            TaskAggregateRepository.GetByIdAsync(Arg.Is<string>(x => Tasks.Any(o => o.Id == x))).Returns(args => Tasks.First(u => u.Id == (string)args[0]));
            TaskAggregateRepository.AddAsync(Arg.Any<Domain.Entities.Task>()).Returns(args => { Tasks.Add((Domain.Entities.Task)args[0]); return Task.CompletedTask; });
            TaskAggregateRepository.UpdateAsync(Arg.Is<Domain.Entities.Task>(x => Tasks.Any(o => o.Id == x.Id))).Returns(args =>
            {
                var existTask = Tasks.FirstOrDefault(u => u.Id == ((Domain.Entities.Task)args[0]).Id);
                if (existTask != null)
                {
                    Tasks.Remove(existTask);
                    Tasks.Add(((Domain.Entities.Task)args[0]));
                }

                return Task.CompletedTask;
            });
            TaskAggregateRepository.ConcurrencySafeUpdate(Arg.Is<Domain.Entities.Task>(x => Tasks.Any(o => o.Id == x.Id)),Arg.Is<string>(x=>x.Any())).Returns(args =>
            {
                var existTask = Tasks.FirstOrDefault(u => u.Id == ((Domain.Entities.Task)args[0]).Id && u.Version == (string)args[1]);
                if (existTask != null)
                {
                    Tasks.Remove(existTask);
                    Tasks.Add(((Domain.Entities.Task)args[0]));
                }

                return Task.CompletedTask;
            });
            TaskAggregateRepository.DeleteAsync(Arg.Is<string>(x => Tasks.Any(o => o.Id == x))).Returns(args =>
            {
                var task = Tasks.FirstOrDefault(u => u.Id == (string)args[0]);
                if (task != null)
                    Tasks.Remove(task);

                return Task.CompletedTask;
            });
            TaskAggregateRepository.GetByCommentIdAsync(Arg.Is<string>(x => x.Any())).Returns(args =>
            {
                return Tasks.FirstOrDefault(u => u.Comments.Any(c => c.Id == (string)args[0]));
            });
        }



        /// <summary>
        /// Each test class should setup its own fixture
        /// </summary>
        protected abstract void TestClassFixtureSetup();



        /// <summary>
        /// 
        /// </summary>
        private List<Domain.Entities.Task> GenerateTasksList()
        {
            var taskValidatorService = Substitute.For<ITaskValidatorService>();
            taskValidatorService.TaskHasUniqueName(taskId: Arg.Any<string>(), boardId: Arg.Any<string>(), taskTitle: Arg.Any<string>()).Returns(true);
            taskValidatorService.CanAddNewTaskToBoard(boardId: Arg.Any<string>(), maxTasksCount: Arg.Any<int>()).Returns(true);
            return TaskObjectMother.GenerateTasksList(taskValidatorService);
        }

    }
}