using _750HrsTracker.DTOs.Requests;
using _750HrsTracker.Enums;
using _750HrsTracker.Helpers;
using _750HrsTracker.Helpers.Constants;
using _750HrsTracker.Models.ActivityLogModels;
using _750HrsTracker.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace _750HrsTracker.Persistence.Seeds
{
    public static class DefaultLogData
    {
        public static async Task SeedDefaultCatgoriesAsync(AppDbContext context)
        {
            var categories = new List<ActivityLogCategory>
            {
                new ActivityLogCategory()
                {
                    Name = "General Activitity",
                    Slug = LogCategoryConstants.GeneralRealEstateSlug,
                    AvailablePropertyType = AvailablePropertyType.LTR
                },
                new ActivityLogCategory()
                {
                    Name = "Material Participation",
                    Slug = LogCategoryConstants.MaterialParticipationSlug,
                    AvailablePropertyType = AvailablePropertyType.LTR
                },
            };

            var existingCategories = await context.ActivityLogCategories.ToListAsync();
            foreach (var category in categories)
            {
                if(!existingCategories.Any(ec => ec.Slug == category.Slug))
                {
                    await context.ActivityLogCategories.AddAsync(category);
                    await context.SaveChangesAsync();   
                }
            }
        }

        public static async Task SeedDefaultLogActivityAsync(AppDbContext _context)
        {
            var activities = new List<SeedLogActivityRequest>
            {
                new SeedLogActivityRequest()
                {
                    Name = "Attempted Property Acquisitions",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.GeneralRealEstateSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Financial Analysis and Feasibility Studies"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Market Research and Due Diligence"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Viewing"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Sourcing for Capital and Funding or Meeting with Lenders"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Travel to view potential markets or properties"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Communications and meetings with Prospective Sellers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Inspections"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Communications and meetings with agents or brokers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Writing Offer Contracts"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Sale Agreements and other legal paperwork review"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Meeting with Investors"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Appraisal Reports Review"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Collaborations on Syndications and Joint Ventures"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "Leasing and Brokerage",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.GeneralRealEstateSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Tenant Recruitment"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Lease Negotiation, Renewals and Execution"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Showings"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Rent Collection"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Tenant and Landlord Relations Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Marketing of Rental Properties"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Client Representation"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Listing Services"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Transaction Coordination"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Networking for Listings and Buyers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Open House Events Marketing"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Market Analysis"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "Real Estate Professional Education and Research",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.GeneralRealEstateSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Real Estate Course Study"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Real Estate License Testing and Certification"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Meeting with Real Estate Coaches and Mentors"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Other Real Estate Educational Activities"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Networking with other Real Estate Investors"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Continuing Education and Certification"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Tax Strategies and Financial Planning"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "General Networking",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.GeneralRealEstateSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Real Estate Clubs and Associations Meeting"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "General Training and Networking Events"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Hosting or Participating in Online Forums"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Attending Industry Conferences and Seminars"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Participating in Educational Workshops and Panels"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "Investor Hours",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.GeneralRealEstateSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Preparing Taxes"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Paying Bills"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Organizing Records and Documents"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "Successful Property Acquisition",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.MaterialParticipationSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Financial Analysis and Feasibility Studies"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Market Research and Due Diligence"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Viewing"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Sourcing for Capital and Funding or Meeting with Lenders"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Travel to view potential markets or properties"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Communications and meetings with Prospective Sellers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Inspections"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Communications and meetings with agents or brokers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Writing Offer Contracts"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Sale Agreements and other legal paperwork review"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Meeting with Investors"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Appraisal Reports Review"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Collaborations on Syndications and Joint Ventures"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "Property Management and Operations",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.MaterialParticipationSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Advertising"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Tenant Screening and Leasing"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Rent Collection and Accounting"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Maintenance and Repairs"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Contractor and Service Provider Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Tenant Relations and Communication"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Eviction Process Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Travel to Property"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Property Inspections"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Showing Property to potential Tenants"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Bills paying and other general administrative tasks"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Other Litigation and Legal Compliances"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Insurance Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Financing Arrangement and Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
                new SeedLogActivityRequest()
                {
                    Name = "Property Construction, Reconstruction, Development, Redevelopment or Conversion",
                    PropertyType = AvailablePropertyType.LTR,
                    LogCategorySlug = LogCategoryConstants.MaterialParticipationSlug,
                    SubCategories = new List<AddUpdateLogActivitySubCategoryRequest>
                    {
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Land Acquisition"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Securing Zoning and Permits"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Design and Architecture Planning"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Contractors and Vendor Selection"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Project Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Contractors and Vendor Supervision"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Financing Arrangements and Capital Raising"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Insurance and Risk Management"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Regulatory Compliance"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Sales and Marketing"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Budgeting and Financial Forecasting"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Market and Feasibility Studies"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Staging and Decorating"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Performing Improvement Work"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Reviewing Sale Agreements"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Communications and Meetings with Buyers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Communications & Meetings with Agents or Brokers"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Advertising and Marketing"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Negotiations"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Other Litigation and Legal Compliances"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Bills paying and other general administrative tasks"
                        },
                        new AddUpdateLogActivitySubCategoryRequest
                        {
                            Name = "Others"
                        }
                    }
                },
            };

            var existingActivities = await _context.ActivityLogActivities.Include(la => la.ActivityLogSubCategories).ToListAsync();
            var existingCategories = await _context.ActivityLogCategories.ToListAsync();

            List<ActivityLogActivity> activityLogActivities = new List<ActivityLogActivity>();

            foreach (var activity in activities)
            {
                var slug = Utility.GenerateSlug(activity.Name!);
                if (!existingActivities.Any(ec => ec.Slug == slug))
                { 
                    var activityData = new ActivityLogActivity()
                    {
                        Name = activity.Name,
                        Slug = slug,
                        AvailablePropertyType = activity.PropertyType,
                       
                    };

                    var category = existingCategories.FirstOrDefault(ec => ec.Slug == activity.LogCategorySlug);
                    if(category != null)
                    {
                        activityData.ActivityLogCategoryId = category.Id;
                    }
                    
                    if(activity.SubCategories != null && activity.SubCategories.Count > 0)
                    {
                        activityData.ActivityLogSubCategories = activity.SubCategories.Select(sc => new ActivityLogSubCategory
                        {
                            Name = sc.Name,
                            Slug = Utility.GenerateSlug(sc.Name!),

                        }).ToList();
                    }


                    await _context.ActivityLogActivities.AddAsync(activityData);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
