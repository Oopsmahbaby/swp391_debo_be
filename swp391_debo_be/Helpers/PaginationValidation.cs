namespace swp391_debo_be.Helpers
{
    public class PaginationValidation
    {
        public static bool ValidatePagination(int page, int limit)
        {
            if (page < 0)
            {
                return false;
            }
            

            if (limit != -1 || limit != 5 || limit != 10 || limit != 25)
            {
                return false;
            }

            return true;
        }
    }
}
