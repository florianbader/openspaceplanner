/**
 * API
 * No description provided (generated by Openapi Generator https://github.com/openapitools/openapi-generator)
 *
 *
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { TimeSlot } from './timeSlot';
import { PageResult } from './pageResult';

export interface TimeSlotItemsResult {
    /**
     * Gets the items.
     */
    items: Array<TimeSlot>;
    pages: PageResult;
}
