using System;

namespace CompManager.Entities

{
  [Flags]
  public enum Role
  {
    ROLE_NONE = 0,
    ROLE_STUDENT = 1,
    ROLE_TEACHER = 2,
    ROLE_ADMIN = 4
  }

}