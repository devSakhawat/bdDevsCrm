(function () {
    'use strict';

    window.SystemSettingsModule = window.SystemSettingsModule || {};
    window.SystemSettingsModule.state = {
        companies: [],
        settingsList: [],
        currentSettings: null,
        assemblyInfo: null
    };
    window.SystemSettingsModule.config = {
        dom: {
            companySelect: '#systemSettingsCompany',
            status: '#systemSettingsStatus',
            form: '#systemSettingsForm',
            assemblyInfo: '#assemblyInfoPanel',
            refreshButton: '#btnRefreshSystemSettings',
            saveButton: '#btnSaveSystemSettings'
        },
        apiEndpoints: {
            companies: `${window.AppConfig.apiBaseUrl}/core/systemadmin/companies-ddl`,
            list: `${window.AppConfig.apiBaseUrl}/core/systemadmin/system-settings`,
            byCompany: function (companyId) { return `${window.AppConfig.apiBaseUrl}/core/systemadmin/system-settings/company/${companyId}`; },
            assemblyInfo: `${window.AppConfig.apiBaseUrl}/core/systemadmin/assembly-info`,
            update: `${window.AppConfig.apiBaseUrl}/core/systemadmin/system-settings`
        },
        sections: [
            { title: 'General Settings', fields: [
                { name: 'settingsId', label: 'Settings Id', type: 'hidden' },
                { name: 'theme', label: 'Theme', type: 'text', maxLength: 100 },
                { name: 'language', label: 'Language', type: 'text', maxLength: 100 },
                { name: 'userId', label: 'User Id', type: 'number', min: 0 },
                { name: 'lastUpdatedDate', label: 'Last Updated Date', type: 'date' },
                { name: 'odbcClientList', label: 'ODBC Client List', type: 'checkbox', valueKind: 'bool' },
                { name: 'specialCharAllowed', label: 'Special Character Allowed', type: 'checkbox', valueKind: 'bool' }
            ] },
            { title: 'Password Policy', fields: [
                { name: 'minLoginLength', label: 'Min Login Length', type: 'number', min: 0 },
                { name: 'minPassLength', label: 'Min Password Length', type: 'number', min: 0 },
                { name: 'passType', label: 'Password Type', type: 'number', min: 0 },
                { name: 'wrongAttemptNo', label: 'Wrong Attempt No', type: 'number', min: 0 },
                { name: 'changePassDays', label: 'Change Password Days', type: 'number', min: 0 },
                { name: 'changePassFirstLogin', label: 'Change Password On First Login', type: 'checkbox', valueKind: 'bool' },
                { name: 'passExpiryDays', label: 'Password Expiry Days', type: 'number', min: 0 },
                { name: 'resetPass', label: 'Reset Password Template', type: 'text', maxLength: 150 },
                { name: 'passResetBy', label: 'Password Reset By', type: 'number', min: 0 },
                { name: 'oldPassUseRestriction', label: 'Old Password Use Restriction', type: 'number', min: 0 },
                { name: 'isPasswordChange', label: 'Password Change Enabled', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isPasswordExpire', label: 'Password Expiry Enabled', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isWebLoginEnable', label: 'Web Login Enabled', type: 'checkbox', valueKind: 'intFlag' }
            ] },
            { title: 'Attendance & Approval', fields: [
                { name: 'isPaddingApplicable', label: 'Padding Applicable', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'deleteApproveLeaveUponPunch', label: 'Delete Approved Leave Upon Punch', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'deleteLateUponAttendanceApproval', label: 'Delete Late Upon Attendance Approval', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isOtLimitApplicable', label: 'OT Limit Applicable', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isSingleBranchApplicable', label: 'Single Branch Applicable', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'checkPreviousAbsenteeism', label: 'Check Previous Absenteeism', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'checkApproverSettings', label: 'Check Approver Settings', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'bypassDefaultStateForSameBoss', label: 'Bypass Default State For Same Boss', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'checkingApproverSettings', label: 'Checking Approver Settings', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableApproverCheckingWhileApplication', label: 'Enable Approver Checking While Application', type: 'checkbox', valueKind: 'intFlag' }
            ] },
            { title: 'Late / Early Exit Rules', fields: [
                { name: 'defaultLateDeductionDaysFirstTime', label: 'Late Deduction Days (First Time)', type: 'number', min: 0 },
                { name: 'defaultLateDeductionDays', label: 'Late Deduction Days', type: 'number', min: 0 },
                { name: 'defaultLateDeductionDaysNext', label: 'Late Deduction Days (Next)', type: 'number', min: 0 },
                { name: 'defaultEarlyExitDeductionDays', label: 'Early Exit Deduction Days', type: 'number', min: 0 },
                { name: 'enableDelayOnShiftInGraceTime', label: 'Enable Delay On Shift-In Grace Time', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableLateAfterShiftInGraceTime', label: 'Enable Late After Shift-In Grace Time', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableEarlyExitBeforeShiftOutGraceTime', label: 'Enable Early Exit Before Shift-Out Grace Time', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableAbsentAfterLateTime', label: 'Enable Absent After Late Time', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableAbsentBeforeEarlyExitTime', label: 'Enable Absent Before Early Exit Time', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableAbsentForNoOutPunch', label: 'Enable Absent For No Out Punch', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'lateTime', label: 'Late Time', type: 'number', min: 0 },
                { name: 'earlyExitTime', label: 'Early Exit Time', type: 'number', min: 0 },
                { name: 'enableCustomStatusOutPunch', label: 'Enable Custom Status For No Out Punch', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'customStatusForNoOutPunch', label: 'Custom Status For No Out Punch', type: 'text', maxLength: 150 },
                { name: 'enableCustomStatusAfterShiftInGraceTime', label: 'Enable Custom Status After Shift-In Grace Time', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'customStatusForAfterShiftinGraceTime', label: 'Custom Status After Shift-In Grace Time', type: 'text', maxLength: 150 },
                { name: 'isLateEarlyExistClearOutAgainstHoliday', label: 'Clear Late/Early Exit Against Holiday', type: 'checkbox', valueKind: 'intFlag' }
            ] },
            { title: 'HR & Payroll', fields: [
                { name: 'isOtCalculateForSalary', label: 'OT Calculate For Salary', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isEmployeeIdAutoGenereted', label: 'Employee Id Auto Generated', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isGradeWiseLeave', label: 'Grade-wise Leave', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'enableMultiplePolicyForSameLeaveType', label: 'Enable Multiple Policy For Same Leave Type', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isAbsenteeismMarge', label: 'Absenteeism Merge', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isTotalBillingApplicable', label: 'Total Billing Applicable', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'isOTCalculateOnHolidayWekend', label: 'OT Calculate On Holiday/Weekend', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'perquisiteLimit', label: 'Perquisite Limit', type: 'number', min: 0 },
                { name: 'isArearFestibleCalculateOnSalary', label: 'Arrear Festival Calculate On Salary', type: 'checkbox', valueKind: 'intFlag' },
                { name: 'regulariseAttendaceDaysLimit', label: 'Regularise Attendance Days Limit', type: 'number', min: 0 },
                { name: 'shortLeaveSlot', label: 'Short Leave Slot', type: 'number', min: 0 },
                { name: 'casualWorkerAmount', label: 'Casual Worker Amount', type: 'number', min: 0, decimals: 2, step: 0.01 },
                { name: 'isHolidayAllowanceCalculate', label: 'Holiday Allowance Calculate', type: 'checkbox', valueKind: 'intFlag' }
            ] }
        ]
    };

    $(document).ready(function () {
        if (!window.AuthManager || !window.AuthManager.isAuthenticated()) {
            window.AppToast?.warning(window.AppConstants?.messages?.warnings?.sessionExpired || 'Session expired');
            setTimeout(function () { window.location.href = '/Account/Login'; }, 1500);
            return;
        }

        window.SystemSettingsModule.Details?.init?.();
        window.SystemSettingsModule.Summary?.init?.();
        $(window.SystemSettingsModule.config.dom.refreshButton).on('click', function () { window.SystemSettingsModule.Summary?.reload?.(); });
        $(window.SystemSettingsModule.config.dom.saveButton).on('click', function () { window.SystemSettingsModule.Details?.save?.(); });
    });
})();
