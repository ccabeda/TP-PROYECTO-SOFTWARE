using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TP_PROYECTO_SOFTWARE.Aplication.DTOs.UserDTOs;
using TP_PROYECTO_SOFTWARE.Aplication.IHandlers;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Commands;
using TP_PROYECTO_SOFTWARE.Aplication.UseCases.Users.Queries;

namespace TP_PROYECTO_SOFTWARE.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    [Tags("Users")]
    public class UsersController : ControllerBase
    {
        private readonly IGetUsersHandler _getUsersHandler;
        private readonly IGetUserByIdHandler _getUserByIdHandler;
        private readonly ICreateUserHandler _createUserHandler;
        private readonly ILoginUserHandler _loginUserHandler;
        private readonly IMapper _mapper;

        public UsersController(
            IGetUsersHandler getUsersHandler,
            IGetUserByIdHandler getUserByIdHandler,
            ICreateUserHandler createUserHandler,
            ILoginUserHandler loginUserHandler,
            IMapper mapper)
        {
            _getUsersHandler = getUsersHandler;
            _getUserByIdHandler = getUserByIdHandler;
            _createUserHandler = createUserHandler;
            _loginUserHandler = loginUserHandler;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Listado de usuarios")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [ProducesResponseType(typeof(List<UserGetDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _getUsersHandler.Handle(new GetUsersQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Obtiene un usuario por id")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [ProducesResponseType(typeof(UserGetDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById([FromRoute] int id)
        {
            var result = await _getUserByIdHandler.Handle(new GetUserByIdQuery { Id = id });
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Crea un usuario")]
        [SwaggerResponse(StatusCodes.Status201Created, "Created")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "Conflict")]
        [ProducesResponseType(typeof(UserGetDTO), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDTO userCreateDTO)
        {
            var command = _mapper.Map<CreateUserCommand>(userCreateDTO);
            var result = await _createUserHandler.Handle(command);
            return CreatedAtAction(nameof(GetUserById), new { id = result.Id }, result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Login simple de usuario")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Not Found")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, "Unauthorized")]
        [ProducesResponseType(typeof(UserLoginResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var command = _mapper.Map<LoginUserCommand>(userLoginDTO);
            var result = await _loginUserHandler.Handle(command);
            return Ok(result);
        }
    }
}
