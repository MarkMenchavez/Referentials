namespace Referentials.Commands;

using Microsoft.AspNetCore.Mvc;
using Referentials.Repositories;

public class DeleteCarCommand
{
    private readonly ICarRepository carRepository;

    public DeleteCarCommand(ICarRepository carRepository) =>
        this.carRepository = carRepository;

    public async Task<IActionResult> ExecuteAsync(int carId, CancellationToken cancellationToken)
    {
        var car = await this.carRepository.GetAsync(carId, cancellationToken).ConfigureAwait(false);
        if (car is null)
        {
            return new NotFoundResult();
        }

        await this.carRepository.DeleteAsync(car, cancellationToken).ConfigureAwait(false);

        return new NoContentResult();
    }
}
