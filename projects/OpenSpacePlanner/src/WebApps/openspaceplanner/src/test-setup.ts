// @ts-expect-error https://thymikee.github.io/jest-preset-angular/docs/getting-started/test-environment
globalThis.ngJest = {
    testEnvironmentOptions: {
        errorOnUnknownElements: true,
        errorOnUnknownProperties: true,
    },
};

import {
    ErrorHandler,
    NgModule,
    provideExperimentalZonelessChangeDetection,
} from '@angular/core';
import { getTestBed, TestBed } from '@angular/core/testing';
import {
    BrowserDynamicTestingModule,
    platformBrowserDynamicTesting,
} from '@angular/platform-browser-dynamic/testing';

import 'jest-extended';
import 'jest-preset-angular/setup-jest';

@NgModule({
    providers: [
        provideExperimentalZonelessChangeDetection(),
        {
            provide: ErrorHandler,
            useValue: {
                handleError: (e: any) => {
                    throw e;
                },
            },
        },
    ],
})
export class TestModule {}

getTestBed().resetTestEnvironment();
TestBed.initTestEnvironment(
    [BrowserDynamicTestingModule, TestModule],
    platformBrowserDynamicTesting(),
);
