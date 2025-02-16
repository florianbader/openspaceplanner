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
import { Topic } from './topic';
import { Room } from './room';

export interface Session {
    id: string;
    name: string;
    rooms: Array<Room>;
    timeSlots: Array<TimeSlot>;
    topics: Array<Topic>;
}
