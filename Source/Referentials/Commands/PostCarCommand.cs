namespace Referentials.Commands;

using Boxed.Mapping;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Referentials.Constants;
using Referentials.Repositories;
using Referentials.ViewModels;

public class PostCarCommand
{
    private readonly IActionContextAccessor actionContextAccessor;
    private readonly ICarRepository carRepository;
    private readonly IMapper<Models.Car, Car> carToCarMapper;
    private readonly IMapper<SaveCar, Models.Car> saveCarToCarMapper;
    private readonly IValidator<SaveCar> saveCarValidator;

    public PostCarCommand(
        IActionContextAccessor actionContextAccessor,
        ICarRepository carRepository,
        IMapper<Models.Car, Car> carToCarMapper,
        IMapper<SaveCar, Models.Car> saveCarToCarMapper,
        IValidator<SaveCar> saveCarValidator)
    {
        this.actionContextAccessor = actionContextAccessor;
        this.carRepository = carRepository;
        this.carToCarMapper = carToCarMapper;
        this.saveCarToCarMapper = saveCarToCarMapper;
        this.saveCarValidator = saveCarValidator;
    }

    public async Task<IActionResult> ExecuteAsync(SaveCar saveCar, CancellationToken cancellationToken)
    {
        var validationResult = await this.saveCarValidator.ValidateAsync(saveCar, cancellationToken).ConfigureAwait(false);
        if (!validationResult.IsValid)
        {
            var modelState = this.actionContextAccessor.ActionContext!.ModelState;
            validationResult.AddToModelState(modelState, null);
            return new BadRequestObjectResult(new ValidationProblemDetails(modelState));
        }

        var car = this.saveCarToCarMapper.Map(saveCar);
        car = await this.carRepository.AddAsync(car, cancellationToken).ConfigureAwait(false);
        var carViewModel = this.carToCarMapper.Map(car);

        return new CreatedAtRouteResult(
            CarsControllerRoute.GetCar,
            new { carId = carViewModel.CarId },
            carViewModel);
    }
}
