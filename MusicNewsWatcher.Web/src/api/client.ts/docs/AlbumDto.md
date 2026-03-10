
# AlbumDto


## Properties

Name | Type
------------ | -------------
`albumId` | number
`artistId` | number
`title` | string
`created` | Date
`isViewed` | boolean
`image` | string
`uri` | string

## Example

```typescript
import type { AlbumDto } from ''

// TODO: Update the object below with actual values
const example = {
  "albumId": null,
  "artistId": null,
  "title": null,
  "created": null,
  "isViewed": null,
  "image": null,
  "uri": null,
} satisfies AlbumDto

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as AlbumDto
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


