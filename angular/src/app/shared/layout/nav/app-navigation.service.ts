import { PermissionCheckerService } from 'abp-ng2-module';
import { AppSessionService } from '@shared/common/session/app-session.service';

import { Injectable } from '@angular/core';
import { AppMenu } from './app-menu';
import { AppMenuItem } from './app-menu-item';

@Injectable()
export class AppNavigationService {
    constructor(
        private _permissionCheckerService: PermissionCheckerService,
        private _appSessionService: AppSessionService
    ) { }

    getMenu(): AppMenu {
        return new AppMenu('MainMenu', 'MainMenu', [
            new AppMenuItem('Dashboard','Pages.Business.Dashboard','esign-dashboard','/app/main/dashboard'),
            new AppMenuItem('Document','Pages.Business.ViewDocument','esign-document','/app/main/document-management'),
            new AppMenuItem('Transfer','Pages.Business.Transfer','esign-transfer','/app/main/transfer'),
            new AppMenuItem('Dashboard', 'Pages.Administration.Host.Dashboard', 'fa-solid fa-desktop', '/app/admin/hostDashboard'),
            new AppMenuItem('Master', 'Pages.Master', 'fa-solid fa-grip', '/app/main/esign/master',[],[
                new AppMenuItem('MasterAffiliate', 'Pages.Master.Affiliate', 'fa-solid fa-grip', '/app/main/esign/master/affiliate'),
                new AppMenuItem('MasterDivision', 'Pages.Master.Division', 'flaticon-suitcase', '/app/main/esign/master/division'),
                new AppMenuItem('Department', 'Pages.Master.Department', 'fa-solid fa-users-line', '/app/main/esign/master/department'),
                new AppMenuItem('Color', 'Pages.Master.EsignColor', 'fa-solid fa-palette', '/app/main/esign/master/color'),
                new AppMenuItem('Status', 'Pages.Master.Status', 'fa-solid fa-check-to-slot', '/app/main/esign/master/status'),
                new AppMenuItem('Systems', 'Pages.Master.System', 'fa-solid fa-bars-progress', '/app/main/esign/master/systems'),
                new AppMenuItem('Category', 'Pages.Master.Category', 'fa-solid fa-grip', '/app/main/esign/master/category'),
                new AppMenuItem('ActivityHistory', 'Pages.Master.Activity', 'fa-solid fa-clock-rotate-left', '/app/main/esign/master/activity-history'),
                // new AppMenuItem('ActiveDirectory', 'Pages.Master.ActiveDirectory', 'fa-solid fa-user-check', '/app/main/esign/master/active-directory'),
                new AppMenuItem('UserImage', 'Pages.Master.SignatureTemplate', 'fa-solid fa-images', '/app/main/esign/master/user-image'),
                new AppMenuItem('SignerTemplate', 'Pages.Master.GroupSigners', 'fa-solid fa-users-between-lines', '/app/main/esign/master/signer-template'),
                new AppMenuItem('ConfigLogo', 'Pages.Master.Logo', 'fa-solid fa-gears', '/app/main/esign/master/config-logo'),
                new AppMenuItem('ConfigEmailTemplate', 'Pages.Master.EmailConfig', 'fa-solid fa-envelope-open-text', '/app/main/esign/master/config-email-template'),
                new AppMenuItem('EsignConfig', 'Pages.Master.EsignConfig', 'fa-solid fa-gear', '/app/main/esign/master/esign-config'),
            ]),

            new AppMenuItem('Tenants', 'Pages.Tenants', 'fa-solid fa-id-card-clip', '/app/admin/tenants'),
            new AppMenuItem('Editions', 'Pages.Editions', 'fa-solid fa-money-bill-trend-up', '/app/admin/editions'),
            new AppMenuItem(
                'Administration', 'Pages.Administration', 'fa-solid fa-list-check', '', [],
                [
                    new AppMenuItem(
                        'OrganizationUnits',
                        'Pages.Administration.OrganizationUnits',
                        'flaticon-map',
                        '/app/admin/organization-units'
                    ),
                    new AppMenuItem('Roles', 'Pages.Administration.Roles', 'flaticon-suitcase', '/app/admin/roles'),
                    new AppMenuItem('Users', 'Pages.Administration.Users', 'fa-solid fa-users-line', '/app/admin/users'),
                    new AppMenuItem('Reports', 'Pages.Administration.AuditReports', 'fa-solid fa-users-line', '/app/admin/reports'),
                    new AppMenuItem(
                        'Languages',
                        'Pages.Administration.Languages',
                        'fa-solid fa-language',
                        '/app/admin/languages',
                        ['/app/admin/languages/{name}/texts']
                    ),
                    new AppMenuItem(
                        'AuditLogs',
                        'Pages.Administration.AuditLogs',
                        'fa-solid fa-book',
                        '/app/admin/auditLogs'
                    ),
                    new AppMenuItem(
                        'DynamicProperties',
                        'Pages.Administration.DynamicProperties',
                        'fa-solid fa-sliders',
                        '/app/admin/dynamic-property'
                    ),
                    new AppMenuItem(
                        'Settings',
                        'Pages.Administration.Host.Settings',
                        'fa-solid fa-gear',
                        '/app/admin/hostSettings'
                    ),
                    new AppMenuItem(
                        'Settings',
                        'Pages.Administration.Tenant.Settings',
                        'fa-solid fa-gear',
                        '/app/admin/tenantSettings'
                    ),
                ]
            ),
        ]);
    }

    checkChildMenuItemPermission(menuItem): boolean {
        for (let i = 0; i < menuItem.items.length; i++) {
            let subMenuItem = menuItem.items[i];

            if (subMenuItem.permissionName === '' || subMenuItem.permissionName === null) {
                if (subMenuItem.route) {
                    return true;
                }
            } else if (this._permissionCheckerService.isGranted(subMenuItem.permissionName)) {
                return true;
            }

            if (subMenuItem.items && subMenuItem.items.length) {
                let isAnyChildItemActive = this.checkChildMenuItemPermission(subMenuItem);
                if (isAnyChildItemActive) {
                    return true;
                }
            }
        }
        return false;
    }

    showMenuItem(menuItem: AppMenuItem): boolean {
        if (
            menuItem.permissionName === 'Pages.Administration.Tenant.SubscriptionManagement' &&
            this._appSessionService.tenant &&
            !this._appSessionService.tenant.edition
        ) {
            return false;
        }

        let hideMenuItem = false;

        if (menuItem.requiresAuthentication && !this._appSessionService.user) {
            hideMenuItem = true;
        }

        if (menuItem.permissionName && !this._permissionCheckerService.isGranted(menuItem.permissionName)) {
            hideMenuItem = true;
        }

        if (this._appSessionService.tenant || !abp.multiTenancy.ignoreFeatureCheckForHostUsers) {
            if (menuItem.hasFeatureDependency() && !menuItem.featureDependencySatisfied()) {
                hideMenuItem = true;
            }
        }

        if (!hideMenuItem && menuItem.items && menuItem.items.length) {
            return this.checkChildMenuItemPermission(menuItem);
        }

        return !hideMenuItem;
    }

    /**
     * Returns all menu items recursively
     */
    getAllMenuItems(): AppMenuItem[] {
        let menu = this.getMenu();
        let allMenuItems: AppMenuItem[] = [];
        menu.items.forEach((menuItem) => {
            allMenuItems = allMenuItems.concat(this.getAllMenuItemsRecursive(menuItem));
        });

        return allMenuItems;
    }

    private getAllMenuItemsRecursive(menuItem: AppMenuItem): AppMenuItem[] {
        if (!menuItem.items) {
            return [menuItem];
        }

        let menuItems = [menuItem];
        menuItem.items.forEach((subMenu) => {
            menuItems = menuItems.concat(this.getAllMenuItemsRecursive(subMenu));
        });

        return menuItems;
    }
}
