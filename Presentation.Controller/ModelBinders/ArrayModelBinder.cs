using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace Presentation.ModelBinders;

public class ArrayModelBinder : IModelBinder
{
  public Task BindModelAsync(ModelBindingContext bindingContext)
  {
    if (!bindingContext.ModelMetadata.IsEnumerableType)
    {
      bindingContext.Result = ModelBindingResult.Failed();
      return Task.CompletedTask;
    }

    var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
    if (string.IsNullOrEmpty(providedValue))
    {
      bindingContext.Result = ModelBindingResult.Success(null);
      return Task.CompletedTask;
    }

    var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
    var converter = TypeDescriptor.GetConverter(genericType);

    try
    {
      var objectArray = providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => converter.ConvertFromString(x.Trim())).ToArray();

      var typedArray = Array.CreateInstance(genericType, objectArray.Length);
      objectArray.CopyTo(typedArray, 0);
      bindingContext.Model = typedArray;

      bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
    }
    catch (Exception ex)
    {
      bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid format.");
      bindingContext.Result = ModelBindingResult.Failed();
      return Task.CompletedTask;
    }

    return Task.CompletedTask;
  }
}

// without error handling

//public class ArrayModelBinder : IModelBinder
//{
//  public Task BindModelAsync(ModelBindingContext bindingContext)
//  {
//    if (!bindingContext.ModelMetadata.IsEnumerableType)
//    {
//      bindingContext.Result = ModelBindingResult.Failed();
//      return Task.CompletedTask;
//    }

//    var providedValue = bindingContext.ValueProvider.Value(bindingContext.ModelName).ToString();
//    if (string.IsNullOrEmpty(providedValue))
//    {
//      bindingContext.Result = ModelBindingResult.Success(null);
//      return Task.CompletedTask;
//    }

//    var genericType = bindingContext.ModelType.TypeInfo().GenericTypeArguments[0];
//    var converter = TypeDescriptor.Converter(genericType);

//    var objectArray = providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
//        .Select(x => converter.ConvertFromString(x.Trim()))
//        .ToArray();

//    var guidArray = Array.CreateInstance(genericType, objectArray.Length);
//    objectArray.CopyTo(guidArray, 0);
//    bindingContext.Model = guidArray;

//    bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
//    return Task.CompletedTask;
//  }
//}
