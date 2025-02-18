using System.Diagnostics.Metrics;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Services
{
    public class TaskService
    {
        // POURQUOI UN SERVICE POURQUOI PAS UN CONTROLLER DIRECTEMENT ? COMMENT UN ALGORITHME PEUT FONCTIONNER EN DEHORS DUN CONTROLLER ? CEST VRAI NORMALEMENT LES MECANISMES
        // FONCTIONNENT DANS LES CONTROLLERS CEST LA BAS QUON A NOS ALGO NOS FONCTIONS ALORS POURQUOI UN DOSSIER SERVICE ET DANS CE DOSSIER UNE CLASS QUI PEUT LUI FAIRE TOURNER
        // DU CODE ET CA SANS CONTROLLER ??


        // Explication :
        // TaskService est responsable de la logique métier des tâches.
        // Il utilise un ITaskRepository pour interagir avec la base de données(qu'on va mocker dans les tests).

        // Ok donc ITaskRepository est une interface propre à .net c'est un nom qui est donné par .Net ou par nous ? Réponse courte

        // EXPLIQUE MOI LE CODE SINON JE NE SERAI PAS CAPABLE DE LE REFAIRE TOUT SEUL SANS TON AIDE EN ENTREPRISE

        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskModel> CreateTaskAsync(TaskModel task)
        {
            return await _taskRepository.CreateTaskAsync(task);
        }

        public async Task<TaskModel> UpdateTaskAsync(TaskModel task)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(task.Id);
            if(existingTask == null)
            {
                return existingTask ?? throw new Exception("Task not found");
            }

            return await _taskRepository.UpdateTaskAsync(task);
        }

        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            return await _taskRepository.DeleteTaskAsync(taskId);
        }

    }
}
