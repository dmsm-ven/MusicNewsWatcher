
# MusicProviderDto


## Properties

Name | Type
------------ | -------------
`musicProviderId` | number
`name` | string
`image` | string
`uri` | string
`totalArtists` | number

## Example

```typescript
import type { MusicProviderDto } from ''

// TODO: Update the object below with actual values
const example = {
  "musicProviderId": null,
  "name": null,
  "image": null,
  "uri": null,
  "totalArtists": null,
} satisfies MusicProviderDto

console.log(example)

// Convert the instance to a JSON string
const exampleJSON: string = JSON.stringify(example)
console.log(exampleJSON)

// Parse the JSON string back to an object
const exampleParsed = JSON.parse(exampleJSON) as MusicProviderDto
console.log(exampleParsed)
```

[[Back to top]](#) [[Back to API list]](../README.md#api-endpoints) [[Back to Model list]](../README.md#models) [[Back to README]](../README.md)


