using FluentValidation;
using Forum.WebAPI.Dto_s;

namespace Forum.WebAPI.Pagination;

public class QueryValidator : AbstractValidator<Query>
{
	private int[] allowedPageSizes = new[] { 5, 10, 15, 30 };
	private string[] allowedSortByColumns = new[] { "Topic", "Date" };

	public QueryValidator()
	{
		RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);

        RuleFor(q => q.PageSize).Custom((value, context) =>
		{
		if (!allowedPageSizes.Contains(value))
			{
				context.AddFailure("PageSize", "PageSize not allowed");
			}
		});

		RuleFor(q => q.SortBy)
			.Must((value) => string.IsNullOrEmpty(value) || allowedSortByColumns.Contains(value))
			.WithMessage($"Sort By is optional, or must be in [{string.Join(",", allowedSortByColumns)}]");
    }
}
