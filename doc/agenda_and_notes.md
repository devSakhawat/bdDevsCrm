# Agenda and Notes

## Current Status (2026-04-18)
- Clean Architecture, frontend layout, এবং naming standards ডকুমেন্টেড (`doc/PROJECT_VISION.md`, `doc/backend_design.md`, `doc/frontend_design.md`, `doc/coding_architecture.md`, `doc/naming_conventions.md`).
- CRUD Records: System মডিউলের রেকর্ড ও ভ্যালিডেটর প্রায় সম্পন্ন (53/54), কিন্তু সার্ভিস/কন্ট্রোলার এখনও DTO + MyMapper নির্ভর; Mapster/Validator ওয়্যারিং বাকি। CRM ও DMS মডিউলের রেকর্ড/ভ্যালিডেটর এখনো শুরু হয়নি (`doc/crud_records_implementation_progress.md`).
- **Latest Fix (2026-04-18)**: ApproverDetailsService এবং ICrmApplicantCourseService এর interface mismatch ফিক্স করা হয়েছে। Build errors 162 থেকে 115-এ নেমে এসেছে।
- আজকের `dotnet build` এখনো ব্যর্থ: মিসিং ভ্যালিডেটর ক্লাস, অনুপস্থিত repository methods (Active*), GridDataSource এক্সটেনশন/হেল্পার অনুপস্থিতির কারণে ~115 টি এরর; অনেক nullable warning-ও আছে। আগে এগুলো ঠিক করতে হবে।

## Next Agenda
1. **System build blockers ফিক্স**: মিসিং ভ্যালিডেটর ক্লাস যোগ/রেজিস্টার, Active* repository মেথড যুক্ত/সমন্বয়, GridDataSource সহায়ক এক্সটেনশন ঠিক করা; রেকর্ড প্রপার্টি নাম মিলিয়ে দেখা।
2. **System services/controllers মাইগ্রেট**: DTO + MyMapper থেকে Records + Mapster + FluentValidation প্যাটার্নে রূপান্তর; একীভূত `ApiResponse<T>` রিটার্ন।
3. **CRM/DMS CRUD Records + Validators জেনারেট**: System প্যাটার্ন অনুসরণ করে ব্যাচ আকারে তৈরি ও ম্যাপিং কনফিগার করা।
4. **Build & CI স্থিতিশীল করা**: প্রতিটি ব্যাচ শেষে `dotnet build`; ApiResponse helper-এ nullable warning কমানো; উপস্থিত টেস্ট চালানো/সংযোজন করা।
5. **Frontend সামঞ্জস্য**: নতুন এন্ডপয়েন্টে Fetch API + Kendo UI 3-ফাইল প্যাটার্ন (settings/details/summary) বজায় রাখা; jQuery শুধুমাত্র DOM/Kendo ইনিশিয়ালাইজেশনে ব্যবহার।

## How We'll Do It
- Mapster ম্যাপিং ও Create/Update/Delete রেকর্ড-ভিত্তিক সার্ভিস ইন্টারফেস ব্যবহার; কন্ট্রোলারে রেকর্ড গ্রহণ ও `ApiResponse<T>` রিটার্ন বজায় রাখা।
- FluentValidation ভ্যালিডেটর (BaseRecordValidator থেকে) প্রতিটি রেকর্ডে প্রয়োগ এবং DI-তে রেজিস্টার।
- Repository-তে প্রয়োজনীয় Active* কুয়েরি যোগ বা সার্ভিস লজিক বিদ্যমান মেথডের সাথে মিলিয়ে সমন্বয়।
- প্রতিটি ব্যাচের পর `dotnet build` চালিয়ে ত্রুটি দ্রুত ধরার অভ্যাস; System সম্পূর্ণ সবুজ না হওয়া পর্যন্ত CRM/DMS এ স্কোপ বাড়ানো হবে না।
- Naming/Layout স্ট্যান্ডার্ড ডক অনুযায়ী রাখা; পারফরম্যান্স টার্গেট (API <500ms P95, UI লোড <2s) মাথায় রেখে ইমপ্লিমেন্টেশন।

## Notes / Follow-ups
- CI এখন ভাঙা; System মডিউলের বিল্ড ব্লকার ঠিক করাই প্রথম অগ্রাধিকার।
- **2026-04-18 Update**: ApproverDetailsService এবং ICrmApplicantCourseService ফিক্স করার পর build errors কমেছে। এখন remaining errors-এ focus করতে হবে।
- CRUD Records প্রগ্রেস ট্র্যাকার (`doc/crud_records_implementation_progress.md`) প্রতিটি ব্যাচ শেষে আপডেট করতে হবে।
