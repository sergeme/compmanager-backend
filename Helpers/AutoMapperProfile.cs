using AutoMapper;
using CompManager.Entities;

namespace CompManager.Helpers
{
  public class AutoMapperProfile : Profile
  {
    // mappings between model and entity objects
    public AutoMapperProfile()
    {
      CreateMap<Account, Models.Accounts.AccountResponse>();

      CreateMap<Account, Models.Accounts.AuthenticateResponse>();

      CreateMap<Account, Models.Competences.CompetencesToReviewResponse>();

      CreateMap<Models.Accounts.RegisterRequest, Account>();

      CreateMap<Models.Accounts.CreateRequest, Account>();

      CreateMap<Models.Accounts.UpdateRequest, Account>()
      .ForAllMembers(x => x.Condition(
          (src, dest, prop) =>
          {
            // ignore null & empty string properties
            if (prop == null) return false;
            if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

            // ignore null role
            if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

            return true;
          }
      ));

      CreateMap<Class, Models.Classes.ClassResponse>();

      CreateMap<Models.Classes.CreateRequest, Class>();

      CreateMap<Models.Classes.UpdateRequest, Class>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Course, Models.Courses.CourseResponse>();

      CreateMap<Models.Courses.CreateRequest, Course>();

      CreateMap<Models.Courses.UpdateRequest, Course>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Competence, Models.Competences.CompetenceResponse>();

      CreateMap<Models.Competences.CreateRequest, Competence>();

      CreateMap<Models.Competences.UpdateRequest, Competence>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Models.Competences.UpdateRequest, Competence>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Models.Courses.ChangeCourseLocationRequest, Course>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Department, Models.Departments.DepartmentResponse>();

      CreateMap<Models.Departments.CreateRequest, Department>();

      CreateMap<Models.Departments.UpdateRequest, Department>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Models.Departments.ChangeDepartmentCourseRequest, Department>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Location, Models.Locations.LocationResponse>();

      CreateMap<Models.Locations.CreateRequest, Location>();

      CreateMap<Models.Locations.UpdateRequest, Location>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Curriculum, Models.Curricula.CurriculumResponse>();

      CreateMap<Models.Curricula.CreateRequest, Curriculum>();

      CreateMap<Models.Curricula.UpdateRequest, Curriculum>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Models.Curricula.ChangeCurriculumProcessTypeRequest, Curriculum>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<ProcessType, Models.ProcessTypes.ProcessTypeResponse>();

      CreateMap<Models.ProcessTypes.CreateRequest, ProcessType>();

      CreateMap<Models.ProcessTypes.UpdateRequest, ProcessType>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));

      CreateMap<Process, Models.Processes.ProcessResponse>();

      CreateMap<Models.Processes.CreateRequest, Process>();

      CreateMap<Models.Processes.UpdateRequest, Process>()
      .ForAllMembers(x => x.Condition(
        (src, dest, prop) =>
        {
          if (prop == null) return false;
          return true;
        }
      ));
    }
  }
}