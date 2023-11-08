using Microsoft.AspNetCore.Mvc;

namespace PostSity.Controllers
{
    public interface IModelController 
    {
        IActionResult ShowTable();
    }
}
