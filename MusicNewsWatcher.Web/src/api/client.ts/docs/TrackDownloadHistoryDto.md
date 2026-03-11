
# TrackDownloadHistoryDto


## Properties

Name | Type
------------ | -------------
`artistName` | string
`albumName` | string
`trackName` | string
`started` | Date
`finished` | Date
`fileSizeInBytes` | number

## Example

```typescript
import type { TrackDownloadHistoryDto } from ''

// TODO: Update the object below with actual values
const example = {
  "artistName": null,
  "albumName": null,
  "trackName": null,
  "started": null,
  "finished": null,
  "fileSizeInBytes": null,
} satisfies TrackDownloadHistoryDto

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as TrackDownloadHistoryDto
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


