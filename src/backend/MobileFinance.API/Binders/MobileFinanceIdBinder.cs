﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sqids;

namespace MobileFinance.API.Binders;

public class MobileFinanceIdBinder : IModelBinder
{
    private readonly SqidsEncoder<long> _idEncoder;

    public MobileFinanceIdBinder(SqidsEncoder<long> idEncoder) => _idEncoder = idEncoder;

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;

        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if(valueProviderResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

        var value = valueProviderResult.FirstValue;

        if(string.IsNullOrWhiteSpace(value))
            return Task.CompletedTask;

        var id = _idEncoder.Decode(value).Single();

        bindingContext.Result = ModelBindingResult.Success(id);

        return Task.CompletedTask;
    }
}
