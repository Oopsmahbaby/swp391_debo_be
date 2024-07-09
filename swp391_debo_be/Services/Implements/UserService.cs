using swp391_debo_be.Constants;
using swp391_debo_be.Cores;
using swp391_debo_be.Dto.Implement;
using swp391_debo_be.Helpers;
using swp391_debo_be.Entity.Implement;
using swp391_debo_be.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using Amazon.S3.Model;
using Amazon.S3;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace swp391_debo_be.Services.Implements
{
    public class UserService : IUserService
    {
        private readonly IAmazonS3 _s3Client;

        public UserService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }
        public ApiRespone CreateUser(CreateUserDto createUserDto)
        {

            if (CUser.GetUserByEmail(createUserDto.Email) != null)
            {
                return new ApiRespone { Success = false, Message = "Email is already exist", StatusCode = System.Net.HttpStatusCode.BadRequest };
            }

            User user = new User()
            {
                Id = System.Guid.NewGuid(),
                Email = createUserDto.Email,
                // Create phone number for user
                Phone = createUserDto.PhoneNumber,
                // Role = 5 la Customer -> dua tren database moi
                Role = 5,
                Gender = true,
                Password = HashPasswordHelper.HashPassword(createUserDto.password),
                IsFirstTime = true
            };

            User createdUser = CUser.CreateUser(user);

            return new ApiRespone { Success = true, Data = createdUser, StatusCode = System.Net.HttpStatusCode.Created };
        }

        public object? GetProfile(string? userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            return CUser.GetUsers();
        }

        public bool IsRefreshToken(User user)
        {
            return CUser.IsRefreshTokenExist(user);
        }

        public async Task<ApiRespone> CreateNewStaff(EmployeeDto employee)
        {
            try
            {
                if (CUser.GetUserByEmail(employee.Email) != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = "Email is already exist", Success = false };
                }
                if (CUser.GetUserByPhoneNumber(employee.Phone) != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = "Phone Number is already exist", Success = false };
                }
                var newStaff = await CUser.CreateNewStaff(employee);
                var staff = await CUser.GetUserById2(newStaff);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = staff, Message = "Create New Staff Successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> CreateNewDent(EmployeeDto employee)
        {
            try
            {
                if (CUser.GetUserByEmail(employee.Email) != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = "Email is already exist", Success = false };
                }
                if (CUser.GetUserByPhoneNumber(employee.Phone) != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = "Phone Number is already exist", Success = false };
                }
                var newDent = await CUser.CreateNewDent(employee);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = newDent, Message = "Create New Dentist Successfully.", Success = true };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> CreateNewManager(EmployeeDto employee)
        {
            try
            {
                if (CUser.GetUserByEmail(employee.Email!) != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Message = "Email is already exist", Success = false };
                }
                if (CUser.GetUserByPhoneNumber(employee.Phone!) != null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = "Phone Number is already exist", Success = false };
                }
                var newMng = await CUser.CreateNewManager(employee);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = newMng, Message = "Create New Manager Successfully.", Success = true };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewStaffList(int page, int limit)
        {
            try
            {
                var data = await CUser.ViewStaffList(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Staff list data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }
        public async Task<ApiRespone> ViewDentList(int page, int limit)
        {
            try
            {
                var data = await CUser.ViewDentList(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Dentist list data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }
        public async Task<ApiRespone> ViewManagerList(int page, int limit)
        {
            try
            {
                var data = await CUser.ViewManagerList(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Manager list data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> ViewCustomerList(int page, int limit)
        {
            try
            {
                var data = await CUser.ViewCustomerList(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Customer list data retrieved successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> GetUserById2(Guid id)
        {
            var response = new ApiRespone();
            try
            {
                var existingUser = await CUser.GetUserById2(id);

                if (!string.IsNullOrEmpty(existingUser.MedRec))
                {
                    // Extract the file key from the MedRec URL
                    var fileKey = new Uri(existingUser.MedRec).AbsolutePath.TrimStart('/');

                    // Fetch metadata from S3
                    var metadata = await _s3Client.GetObjectMetadataAsync(new GetObjectMetadataRequest
                    {
                        BucketName = "swp391-bucket",
                        Key = fileKey
                    });
                    // Them Name cua file
                    existingUser.MedRecMetaData = new MedRecMetaDataDto
                    {
                        NameFile = fileKey,
                        FileSize = metadata.ContentLength,
                        LastModified = metadata.LastModified,
                        ContentType = metadata.Headers.ContentType
                    };
                }

                var role = existingUser.RoleName;
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = existingUser, Message = role + " data retrieved successfully.", Success = true };
            }
            catch(Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
            return response;
        }

        public async Task<ApiRespone> UpdateUser(Guid id, EmployeeDto emp)
        {
            try
            {
                if (id != emp.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User ID mismatch.", Success = false };
                }
                //if (CUser.GetUserByEmail(emp.Email) != null)
                //{
                //    response.StatusCode = HttpStatusCode.BadRequest;
                //    response.Success = false;
                //    response.Message = "Email is already exist";
                //    return response;
                //}
                //if (CUser.GetUserByPhoneNumber(emp.Phone) != null)
                //{
                //    response.StatusCode = HttpStatusCode.BadRequest;
                //    response.Success = false;
                //    response.Message = "Phone Number is already exist";
                //    return response;
                //}
                var data = await CUser.GetUserById2(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User not found", Success = false };
                }
                await CUser.UpdateUser(id, emp);
                var updUser = await CUser.GetUserById2(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updUser, Message = "User data updated successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> UploadAvatarUser(Guid id, EmployeeDto emp)
        {
            try
            {
                if (id != emp.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User ID mismatch.", Success = false };
                }
                var data = await CUser.GetUserById2(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User not found.", Success = false };
                }
                await CUser.UploadAvatarUser(id, emp);
                var updUser = await CUser.GetUserById2(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updUser, Message = "Avatar profile uploaded successfully.", Success = true };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> UploadMedRecPatient(Guid id, EmployeeDto emp)
        {
            try
            {
                if (id != emp.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User ID mismatch.", Success = false };
                }
                var data = await CUser.GetUserById2(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User not found.", Success = false };
                }
                await CUser.UploadMedRecPatient(id, emp);
                var updUser = await CUser.GetUserById2(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updUser, Message = "Medical Record file uploaded successfully.", Success = true };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> UpdatePassword(Guid id, EmployeeDto emp)
        {
            try
            {
                if (id != emp.Id)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User ID mismatch.", Success = false };
                }
                var data = await CUser.GetUserById2(id);
                if (data == null)
                {
                    return new ApiRespone { StatusCode = HttpStatusCode.NotFound, Message = "User not found.", Success = false };
                }
                await CUser.UpdatePassword(id, emp);
                var updUser = await CUser.GetUserById2(id);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = updUser, Message = "User data updated successfully.", Success = true };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }
            
        public ApiRespone firstTimeBooking(string userId)
        {
            try
            {
                if (Guid.TryParse(userId, out Guid id))
                {
                    var respone = CUser.firstTimeBooking(id);

                    return new ApiRespone { StatusCode = HttpStatusCode.OK,Data = respone, Message = "Fetched first time booking successfully.", Success = true };
                } else
                {
                       return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = "Invalid user id", Success = false };
                }
            } catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> AvailableManager(int page, int limit)
        {
            try
            {
                var data = await CUser.AvailableManager(page, limit);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = new { list = data, total = data.Count }, Message = "Manager list data retrieved successfully.", Success = true };

            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public async Task<ApiRespone> CreateDentistMajor(DentistMajorDto dentmaj)
        {
            try
            {
                var data = await CUser.CreateDentistMajor(dentmaj);
                return new ApiRespone { StatusCode = HttpStatusCode.OK, Data = data, Message = "Create dentist major successfully.", Success = true };
            }
            catch (Exception ex)
            {
                return new ApiRespone { StatusCode = HttpStatusCode.BadRequest, Data = null, Message = ex.Message, Success = false };
            }
        }

        public bool ValidAdminEmail(string email)
        {
            try
            {
                return CUser.ValidAdminEmail(email);

            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
