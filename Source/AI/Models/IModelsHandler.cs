namespace DotNetToolbox.AI.Models;

public interface IModelsHandler
    : IModelsHandler<string>;

/// <summary>
/// This class contains methods for handling models using OpenAI.
/// </summary>
/// <remarks>
/// This class is responsible for handling models using OpenAI. It contains methods for getting a list of models, getting a model by ID, and deleting a model. It also includes a detailed XML documentation for each method.
/// </remarks>
public interface IModelsHandler<TModel> {
    /// <summary>
    /// Gets a list of models ids of the specified type.
    /// </summary>
    /// <param name="type">The type of the models to get.</param>
    /// <returns>An array of strings with the ids of models of the specified type.</returns>
    /// <remarks>
    /// This method gets a list of model's id of the specified type from the OpenAI API.
    /// </remarks>
    Task<string[]> GetIds(string? type = null);
}
