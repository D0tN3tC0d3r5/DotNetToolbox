﻿namespace DotNetToolbox.ValidationBuilder;

public interface IConnector<out TValidator>
    : ITerminator,
      IBinaryConnector<TValidator>,
      IBinaryOperator<TValidator>
    where TValidator : IValidator;
