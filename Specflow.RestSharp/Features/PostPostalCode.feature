Feature: Postal Codes

@PostRun
  Scenario: Post PostalCodes - 200
    Given path 'postcodes'
     When method POST
      And headers
      | Key	     | Value	        | 
      | Content-Type | application/json | 
      And payload "PostDataPayload"
  """
  {
  "postcodes" : ["Postal Codes to get added to payload"]
  }
  """
     Then status "200"
     Then the schema is correct "PostDataResponseSchema"
  
