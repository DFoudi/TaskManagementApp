using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;
using TaskManagementApp.Services;

namespace TaskManagementApp.Tests.Unit.Services
{
    [TestClass]
    public class TaskServiceTests
    {
        private Mock<ITaskRepository> _taskRepositoryMock;
        private TaskService _taskService;

        [TestInitialize]
        public void Setup()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [TestMethod]
        public async Task CreateTask_ShouldReturnTask_WhenValidInput()
        {
            // Arrange
            var newTask = new TaskModel { Name = "Test Task", IsCompleted = false };
            _taskRepositoryMock.Setup(repo => repo.CreateTaskAsync(It.IsAny<TaskModel>()))
                .ReturnsAsync(newTask);

            // Act
            var result = await _taskService.CreateTaskAsync(newTask);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Task", result.Name);
            _taskRepositoryMock.Verify(repo => repo.CreateTaskAsync(It.IsAny<TaskModel>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateTask_ShouldReturnUpdatedTask_WhenValidInput()
        {
            // Arrange
            var taskId = new Random().Next(1, 1000);
            var existingTask = new TaskModel { Id = taskId, Name = "Old Task", IsCompleted = false };
            var updatedTask = new TaskModel { Id = taskId, Name = "Updated Task", IsCompleted = true };

            _taskRepositoryMock.Setup(repo => repo.GetTaskByIdAsync(taskId))
                .ReturnsAsync(existingTask);

            _taskRepositoryMock.Setup(repo => repo.UpdateTaskAsync(It.IsAny<TaskModel>()))
                .ReturnsAsync((TaskModel t) => t); // Retourne directement l'instance mise à jour

            // Act
            var result = await _taskService.UpdateTaskAsync(updatedTask);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Task", result.Name);
            Assert.IsTrue(result.IsCompleted);
            _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskModel>()), Times.Once);
        }

        [TestMethod]
        public async Task UpdateTask_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = new Random().Next(1, 1000);
            var updatedTask = new TaskModel { Name = "Updated Task", IsCompleted = true };

            _taskRepositoryMock
                .Setup(repo => repo.GetTaskByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((TaskModel?)null); // Ajoute explicitement le type nullable

            // Act
            var result = await _taskService.UpdateTaskAsync(updatedTask);

            // Assert
            Assert.IsNull(result);
            _taskRepositoryMock.Verify(repo => repo.UpdateTaskAsync(It.IsAny<TaskModel>()), Times.Never);
        }

        [TestMethod]
        public async Task DeleteTask_ShouldReturnTrue_WhenTaskExists()
        {
            // Arrange
            _taskRepositoryMock.Setup(repo => repo.DeleteTaskAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _taskService.DeleteTaskAsync(1);

            // Assert
            Assert.IsTrue(result);
            _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(1), Times.Once);
        }

        [TestMethod]
        public async Task DeleteTask_ShouldReturnFalse_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = new Random().Next(1, 1000);

            _taskRepositoryMock.Setup(repo => repo.DeleteTaskAsync(taskId))
                .ReturnsAsync(false); // Simule une suppression impossible

            // Act
            var result = await _taskService.DeleteTaskAsync(taskId);

            // Assert
            Assert.IsFalse(result);
            _taskRepositoryMock.Verify(repo => repo.DeleteTaskAsync(taskId), Times.Once);
        }

    }
}
