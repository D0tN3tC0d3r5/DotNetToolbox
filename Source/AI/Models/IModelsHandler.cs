namespace DotNetToolbox.AI.Models;

/// <summary>
/// This class contains methods for handling models using OpenAI.
/// </summary>
/// <remarks>
/// This class is responsible for handling models using OpenAI. It contains methods for getting a list of models, getting a model by ID, and deleting a model. It also includes a detailed XML documentation for each method.
/// </remarks>
public interface IModelsHandler {
    /// <summary>
    /// Gets a list of models of the specified type.
    /// </summary>
    /// <param name="type">The type of the models to get.</param>
    /// <returns>An array of models of the specified type.</returns>
    /// <remarks>
    /// This method gets a list of models of the specified type from the OpenAI API.
    /// </remarks>
    Task<Model[]> Get(ModelType type = ModelType.Chat);

    /// <summary>
    /// Gets the model with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the model to get.</param>
    /// <returns>The model with the specified ID.</returns>
    /// <remarks>
    /// This method gets the model with the specified ID from the OpenAI API.
    /// </remarks>
    Task<Model?> GetById(string id);

    /// <summary>
    /// Deletes the model with the specified ID.
    /// </summary>
    /// <param name="id">The ID of the model to delete.</param>
    /// <returns>A boolean indicating whether the model was deleted successfully.</returns>
    /// <remarks>
    /// This method deletes the model with the specified ID from the OpenAI API.
    /// </remarks>
    Task<bool> Delete(string id);
}
